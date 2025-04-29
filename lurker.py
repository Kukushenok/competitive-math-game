import os
import re
import sys

def remove_comments(content):
    # Remove single-line comments
    content = re.sub(r'//.*', '', content)
    # Remove multi-line comments
    content = re.sub(r'/\*.*?\*/', '', content, flags=re.DOTALL)
    return content

def find_namespaces(content):
    namespaces = []
    pos = 0
    while True:
        match = re.search(r'namespace\s+([\w\.]+)\s*\{', content[pos:], re.MULTILINE)
        if not match:
            break
        namespace_name = match.group(1)
        start = pos + match.start()
        brace_start = pos + match.end() - 1  # position of '{'
        brace_count = 1
        end = brace_start + 1
        while end < len(content) and brace_count > 0:
            if content[end] == '{':
                brace_count += 1
            elif content[end] == '}':
                brace_count -= 1
            end += 1
        namespace_content = content[brace_start + 1 : end - 1]
        namespaces.append( (namespace_name, namespace_content) )
        pos = end
    # Add global content
    global_content = content[pos:]
    if global_content.strip():
        namespaces.append( (None, global_content) )
    return namespaces

def find_classes(content):
    classes = []
    pos = 0
    while True:
        match = re.search(r'(class|interface)\s+([\w<>]+)\s*(:\s*([\w\.,\s<>]+))?\s*\{', content[pos:], re.MULTILINE)
        if not match:
            break
        class_name = match.group(1)
        base_classes = []
        if match.group(3):
            base_classes = [bc.strip() for bc in match.group(3).split(',')]
        # Find class body
        start = pos + match.end() - 1  # position of '{'
        brace_count = 1
        end = start + 1
        while end < len(content) and brace_count > 0:
            if content[end] == '{':
                brace_count += 1
            elif content[end] == '}':
                brace_count -= 1
            end += 1
        class_body = content[start + 1 : end - 1]
        classes.append( (class_name, base_classes, class_body) )
        pos = end
    return classes

def parse_public_members(class_body):
    members = {'fields': [], 'properties': [], 'methods': []}
    
    # Fields: public Type Name;
    field_pattern = r'public\s+([\w<>]+)\s+(\w+)\s*;'
    for match in re.finditer(field_pattern, class_body):
        type_name = match.group(1)
        field_name = match.group(2)
        members['fields'].append(f"{type_name} {field_name}")
    
    # Properties: public Type Name { get; [set;] }
    prop_pattern = r'public\s+([\w<>]+)\s+(\w+)\s*{\s*get;\s*(set;)?\s*}'
    for match in re.finditer(prop_pattern, class_body):
        type_name = match.group(1)
        prop_name = match.group(2)
        has_set = match.group(3) is not None
        prop_str = f"{type_name} {prop_name} {{get; set;}}" if has_set else f"{type_name} {prop_name} {{get;}}"
        members['properties'].append(prop_str)
    
    # Methods: public ReturnType Name<Generics>(Parameters)
    method_pattern = r'public\s+([\w<>]+)\s+(\w+)\s*(<([\w\s,]+)>)?\s*\(([^)]*)\)'
    for match in re.finditer(method_pattern, class_body):
        return_type = match.group(1)
        method_name = match.group(2)
        generics = match.group(4)
        params = match.group(5).strip()
        
        if generics:
            method_name += f"<{generics}>"
        params_list = []
        if params:
            for param in params.split(','):
                param = param.strip()
                if param:
                    parts = param.rsplit(' ', 1)
                    if len(parts) == 2:
                        param_type, param_name = parts
                        params_list.append(f"{param_type.strip()} {param_name.strip()}")
                    else:
                        params_list.append(param)
        params_str = ', '.join(params_list)
        members['methods'].append(f"{return_type} {method_name}({params_str})")
    
    return members

def process_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    cleaned_content = remove_comments(content)
    namespaces = find_namespaces(cleaned_content)
    for ns_name, ns_content in namespaces:
        classes = find_classes(ns_content)
        for class_name, base_classes, class_body in classes:
            members = parse_public_members(class_body)
            print(f"Class: {class_name}")
            if ns_name:
                print(f"Namespace: {ns_name}")
            if base_classes:
                print(f"Derived from: {', '.join(base_classes)}")
            print("Public members:")
            if members['fields']:
                print("Fields:")
                for field in members['fields']:
                    print(f"  {field}")
            if members['properties']:
                print("Properties:")
                for prop in members['properties']:
                    print(f"  {prop}")
            if members['methods']:
                print("Methods:")
                for method in members['methods']:
                    print(f"  {method}")
            print()

def main():
    if len(sys.argv) != 2:
        print("Usage: python csharp_parser.py <directory>")
        sys.exit(1)
    
    directory = sys.argv[1]
    if not os.path.isdir(directory):
        print(f"Error: {directory} is not a valid directory")
        sys.exit(1)
    
    cs_files = []
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                cs_files.append(os.path.join(root, file))
    
    for cs_file in cs_files:
        process_file(cs_file)

if __name__ == "__main__":
    main()