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

function closeRegisterPage() {
    const registerPage = document.getElementById('register-page');
    registerPage.style.display = 'none'; // Oculta a página de registro
}

function closeLoginPage() {
    const loginPage = document.getElementById('login-page');
    loginPage.style.display = 'none'; // Oculta a página de login
}