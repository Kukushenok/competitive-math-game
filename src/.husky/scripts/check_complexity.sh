#!/usr/bin/env bash
set -e

echo "➡️ Checking cyclomatic complexity (limit=10)..."

# Флаг для ошибок
violations=0

# Найти все .Metrics.xml
find . -type f -name "*.Metrics.xml" | while read -r file; do
    # Извлекаем пары "Name" и "CyclomaticComplexity" из XML
    while IFS= read -r line; do
        name=$(echo "$line" | sed -n 's/.*Name="\([^"]*\)".*/\1/p')
        complexity=$(echo "$line" | sed -n 's/.*CyclomaticComplexity="\([0-9]*\)".*/\1/p')
        if [ -n "$complexity" ] && [ "$complexity" -gt 10 ]; then
            echo "❌ $name in $file has CyclomaticComplexity=$complexity"
            violations=1
        fi
    done < <(grep 'CyclomaticComplexity' "$file")
done

if [ "$violations" -eq 1 ]; then
    echo "❌ Cyclomatic complexity exceeded (limit=10)"
    exit 1
else
    echo "✅ Cyclomatic complexity within limits"
fi
