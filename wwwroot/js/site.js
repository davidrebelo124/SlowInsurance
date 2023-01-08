// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Toggle Password
const togglePassword = document.querySelector('#togglePassword');
const password = document.querySelector('#password');

togglePassword.addEventListener('click', function (e) {
    // toggle the type attribute
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    // toggle the eye slash icon
    this.classList.toggle('fa-eye-slash');
});

//Toggle confirm password
const toggleConfirmPassword = document.querySelector('#toggleConfirmPassword');
const confirmpassword = document.querySelector('#confirmpassword');

toggleConfirmPassword.addEventListener('click', function (e) {
    // toggle the type attribute
    const type = confirmpassword.getAttribute('type') === 'password' ? 'text' : 'password';
    confirmpassword.setAttribute('type', type);
    // toggle the eye slash icon
    this.classList.toggle('fa-eye-slash');
});

//Toggle current password
const toggleCurrentPassword = document.querySelector('#toggleCurrentPassword');
const currentpassword = document.querySelector('#currentpassword');

toggleCurrentPassword.addEventListener('click', function (e) {
    // toggle the type attribute
    const type = currentpassword.getAttribute('type') === 'password' ? 'text' : 'password';
    currentpassword.setAttribute('type', type);
    // toggle the eye slash icon
    this.classList.toggle('fa-eye-slash');
});

// enable Tooltips
document.querySelectorAll('[data-bs-toggle="tooltip"]')
    .forEach(tooltip => {
        new bootstrap.Tooltip(tooltip)
    })