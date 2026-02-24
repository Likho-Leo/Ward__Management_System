// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById('togglePassword').addEventListener('click', function () {
    var password = document.getElementById('passwordInput');
    if (password.type === 'password') {
        password.type = 'text';
        this.classList.remove('fa-eye');
        this.classList.add('fa-eye-slash');
    } else {
        password.type = 'password';
        this.classList.remove('fa-eye-slash');
        this.classList.add('fa-eye');
    }
});

document.getElementById('togglePasswordConfi').addEventListener('click', function () {
    var password = document.getElementById('passwordInputConf');
    if (password.type === 'password') {
        password.type = 'text';
        this.classList.remove('fa-eye');
        this.classList.add('fa-eye-slash');
    } else {
        password.type = 'password';
        this.classList.remove('fa-eye-slash');
        this.classList.add('fa-eye');
    }
});

$(document).ready(function () {
    // Example of attaching an event listener
    const nextButton = document.getElementById('nextToMedical');
    if (nextButton) {
        nextButton.addEventListener('click', function () {
            // Your code here
        });
    }
});

