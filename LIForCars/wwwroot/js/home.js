document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('registrationForm');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault(); // Impede o envio imediato do formulário

            var nif = document.getElementById('registerNif').value;
            var cc = document.getElementById('registerCc').value;
            var phone = document.getElementById('registerPhone').value;
            var username = document.getElementById('registerUsername').value;
            var email = document.getElementById('registerEmail').value;
            var password = document.getElementById('registerPassword').value;
            var confirmPassword = document.getElementById('registerConfirmPassword').value;

            let errors = [];

            if (!/^\d+$/.test(nif)) errors.push('NIF must be a number.');
            if (!/^\d+$/.test(cc)) errors.push('CC must be a number.');
            if (!/^\d+$/.test(phone)) errors.push('Phone must be a number.');
            if (/\s/.test(username)) errors.push('Username cannot contain spaces.');
            if (password !== confirmPassword) errors.push('Passwords do not match.');

            // Realiza as verificações de unicidade em paralelo
            const uniqueChecks = await Promise.all([
                isUnique('nif', nif),
                isUnique('cc', cc),
                isUnique('phone', phone),
                isUnique('username', username),
                isUnique('email', email)
            ]);

            if (!uniqueChecks[0]) errors.push('NIF already in use.');
            if (!uniqueChecks[1]) errors.push('CC already in use.');
            if (!uniqueChecks[2]) errors.push('Phone already in use.');
            if (!uniqueChecks[3]) errors.push('Username already in use.');
            if (!uniqueChecks[4]) errors.push('Email already in use.');

            if (errors.length > 0) {
                alert(errors.join('\n'));
                e.preventDefault();
                return;
            }

            form.submit(); // Envie o formulário se não houver erros
        });
    }
});

async function isUnique(field, value) {
    try {
        const response = await fetch(`/api/User/checkUnique?field=${field}&value=${value}`);
        if (!response.ok) {
            throw new Error('Erro ao verificar a unicidade do campo');
        }
        return await response.json();
    } catch (error) {
        console.error('Erro na verificação de unicidade:', error);
        return false; // Considera não único em caso de erro
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('loginForm');
    if (form) {
        form.addEventListener('submit', async function(e) {
            e.preventDefault(); // Impede o envio imediato do formulário

            var username = document.getElementById('loginUsername').value;
            var password = document.getElementById('loginPassword').value;

            console.log(username);
            console.log(password);

            let errors = [];

            const check = await checkPassword(username, password);
            if (!check) {
                alert('Username or password incorrect.');
                e.preventDefault();
                return;
            }


            form.submit(); // Envie o formulário se não houver erros
        });
    }
});

async function checkPassword(username, password) {
    try {
        const response = await fetch(`/api/User/checkPassword?username=${username}&password=${password}`);
        if (!response.ok) {
            throw new Error('Erro ao verificar a unicidade do campo');
        }
        return await response.json();
    } catch (error) {
        console.error('Erro na verificação de unicidade:', error);
        return false; // Considera não único em caso de erro
    }
}

function showLoginPage() {
    document.getElementById('login-page').style.display = 'block';
}

function closeLoginPage() {
    document.getElementById('login-page').style.display = 'none';
}

function showRegisterPage() {
    closeLoginPage();
    document.getElementById('register-page').style.display = 'block';
}

function closeRegisterPage() {
    document.getElementById('register-page').style.display = 'none';
}

function closeAboutPage() {
    document.getElementById('about-page').style.display = 'none';
}

function showAboutPage() {
    document.getElementById('about-page').style.display = 'block';
}

function closeContactPage() {
    document.getElementById('contact-page').style.display = 'none';
}

function showContactPage() {
    document.getElementById('contact-page').style.display = 'block';
}

document.addEventListener("DOMContentLoaded", () => {
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('nav a');
    
    navLinks.forEach(link => {
        if(link.getAttribute('href') === currentPath) {
            link.classList.add('active'); // Ensure you define the .active class in your CSS
        }
    });
});