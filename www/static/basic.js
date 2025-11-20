let INJECTED_DATA = [];


function openInNewTab(url) {
 window.open(url, '_blank').focus();
}

function createArticle(element){
	let name = element['image'];
	let mcontent = document.getElementById("maincontent");
	let A = document.createElement("article");
	A.className="browser";
	A.classList.add("doupel");
	let icon = document.createElement("img");
	icon.src = element['icon'];
	icon.style.height = "50px";
	icon.style.width = "auto";
	
	A.appendChild(icon);
	
	if(element['specialImagePath']=='none'){
		let HName = document.createElement("span");
		HName.appendChild(document.createTextNode(name));
		HName.style.fontSize="40px";
		A.appendChild(HName);
	}else{
		let HName = document.createElement("img");
		HName.src = element['specialImagePath'];
		HName.style.height = "50px";
		HName.style.width = "auto";
		
		A.appendChild(HName);
	}
	A.onclick = function(){
		if(element["specialHTMLPath"]=='none') openInNewTab("mainviewer.html?image="+element["image"]+"&close=y");
		else window.location = element["specialHTMLPath"];
	}
	mcontent.appendChild(A);
}
let body = document.getElementById("background"); 
let milkplayed = false;
//
//
//INIT
//
//

let INITIALIZEDTED_ALREADY = false;
function init(){
	if(INITIALIZEDTED_ALREADY) return;
	INITIALIZEDTED_ALREADY = true;
	let mcontent = document.getElementById("maincontent");
	mcontent.hidden = false;
	let start = Date.now();
	//let amogn = document.getElementById("intruder");
	let interval = 1;
	function getRandomInt(max) {
	  return Math.floor(Math.random() * max);
	}
	body.style.position = "fixed";
	for(let i = 0; i<interval*20; i+=interval){
		for(let j = 0; j<interval*20; j+=interval){
			let img = document.createElement("img");
			img.style.overflow = "hidden";
			img.style.userSelect = "none";
			img.style.userDrag = "none";
			img.style.webkitUserSelect = "none";
			img.style.mozUserSelect = "none";
			img.src = "CONTENT/intruder.gif";
			body.appendChild(img);
			img.style.position='absolute';
			//img.style.left = i+"px";
		    //img.style.top = j+"px";
			//img.I = i;
			//let dX = img.style.offset.left;
			//let dY = img.style.offset.top;
			
			img.style.position='relative';
			img.style.left = i+"px";
			img.style.top = j+"px";
			//img.J = j;
			//let IMG_DATA = {"i":i,"j":j}
			
			//img.display = 'flex';
			img.style.zIndex=7;
			let sus = getRandomInt(8)+3;
			let startAngle = getRandomInt(360);
			img.classList.add("specimen");
			img.style.animationDuration = (1.5 + Math.random() * 3.0) * 5.0+ 's';
			
		}
	}
	INJECTED_DATA.forEach(elem => {
		createArticle(elem);
	})
}
//
//
//TRYINIT
//
//
var audio = new Audio('CONTENT/milk2.mp3');
let playmilk = function() {
	if(milkplayed) return;
	var playedPromise = audio.play();
	if (playedPromise) {
		trycatch = true;
        playedPromise.catch((e) => {
        	 trycatch = false;
             console.log(e.name);
             
             if (e.name !== 'NotAllowedError' && e.name !== 'NotSupportedError') { 
             	   
              }
         }).then(() => {
            if(trycatch){
	        	milkplayed = true;
	        	clearInterval(initializationprocess);
	        	init();
        	}
         });

     }
     
}
let initializationprocess = setInterval(playmilk,200);



