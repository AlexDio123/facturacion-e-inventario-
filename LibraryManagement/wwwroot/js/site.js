// Write your JavaScript code.
let username = "admin";
let password = "1234";

function user() {
    let user = document.getElementById("usuario");
    let pass = document.getElementById("password");
    if (user.value === username && pass.value === password) {
        
        window.location.href = "http://localhost:14871/Home/index";

    } else {
        alert("usuario o contraseña incorrecta!");
    } 
        
}