function GetURLParameter(sParam)
{
    let sPageURL = window.location.search.substring(1);
    let sURLVariables = sPageURL.split('&');
    for (let i = 0; i < sURLVariables.length; i++) 
    {
        let sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) 
        {
            return sParameterName[1];
        }
    }
    return 'none';
}
let imgname = GetURLParameter("image");
let body = document.getElementById("maincontent");
let label = document.createElement("h1");
label.appendChild(document.createTextNode(imgname));
body.appendChild(label);
if(imgname!='none'){
	console.log(imgname);
	let img = document.createElement("img");
	img.src = "CONTENT/INJECTED/"+imgname;
	img.className  = "center";
	img.style.height = "420px";
	img.style.width = "auto";
	body.appendChild(img);
}
let button = document.getElementById("goback");
let musicname = GetURLParameter("music");
if(musicname!='none'){
	let msc = document.createElement("audio");
	msc.controls = true;
	msc.src = musicname;
	msc.className  = "center";
	body.appendChild(msc);
}
let backColor = GetURLParameter("backColor");
console.log(backColor);
if(backColor!='none'){
	document.body.style.backgroundColor = "#"+backColor;
}
let close = GetURLParameter("close");
if(close=='none'){
	button.onclick = function(){
	window.location = "index.html";
	}
}else{
	button.onclick = function(){
	window.close();
	}
}