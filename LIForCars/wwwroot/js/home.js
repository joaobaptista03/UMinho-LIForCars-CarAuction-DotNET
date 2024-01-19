document.addEventListener('DOMContentLoaded', function () {
    const registerLink = document.querySelector('[data-target="register"]');
    const loginLink = document.querySelector('[data-target="login"]');
    
    // Elementos das páginas de registro e login
    const registerPage = document.querySelector('#register-page'); // Substitua com o ID real da página de registro
    const loginPage = document.querySelector('#login-page'); // Substitua com o ID real da página de login
    
    // Event listener para mostrar a página de registro quando o link de registro é clicado
    registerLink.addEventListener('click', function (e) {
        e.preventDefault(); // Evita o redirecionamento padrão
        hidePages(); // Oculta todas as páginas
        registerPage.style.display = 'block'; // Mostra a página de registro
    });
    
    // Event listener para mostrar a página de login quando o link de login é clicado
    loginLink.addEventListener('click', function (e) {
        e.preventDefault(); // Evita o redirecionamento padrão
        hidePages(); // Oculta todas as páginas
        loginPage.style.display = 'block'; // Mostra a página de login
    });
    
    // Função para ocultar todas as páginas
    function hidePages() {
        registerPage.style.display = 'none';
        loginPage.style.display = 'none';
        // Oculte outras páginas, se houver
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('registrationForm');
    if (form) {
        form.addEventListener('submit', function(e) {
            var nif = document.getElementById('nif').value;
            var cc = document.getElementById('cc').value;
            var phone = document.getElementById('phone').value;
            var password = document.getElementById('password').value;
            var confirmPassword = document.getElementById('confirmPassword').value;

            // Validação do NIF
            if (!/^\d+$/.test(nif)) {
                alert('NIF must be a number.');
                e.preventDefault(); // Impede o envio do formulário
                return;
            }

            // Validação do CC
            if (!/^\d+$/.test(cc)) {
                alert('CC must be a number.');
                e.preventDefault(); // Impede o envio do formulário
                return;
            }

            // Validação do Telemóvel
            if (!/^\d+$/.test(phone)) {
                alert('Telemóvel must be a number.');
                e.preventDefault(); // Impede o envio do formulário
                return;
            }

            // Validação da Senha
            if (password !== confirmPassword) {
                alert('Passwords do not match.');
                e.preventDefault(); // Impede o envio do formulário
            }

            // 
        });
    }
});

function closeRegisterPage() {
    const registerPage = document.getElementById('register-page');
    registerPage.style.display = 'none'; // Oculta a página de registro
}

function closeLoginPage() {
    const loginPage = document.getElementById('login-page');
    loginPage.style.display = 'none'; // Oculta a página de login
}