function showAboutPage() {
    $.ajax({
        url: '/Index?handler=AboutPartial',
        type: 'GET',
        success: function (data) {
            $('#about-section').html(data);
            $('#about-section').show();
        }
    });
}

function showContactPage() {
    $.ajax({
        url: '/Index?handler=ContactPartial',
        type: 'GET',
        success: function (data) {
            $('#contact-section').html(data);
            $('#contact-section').show();
        }
    });
}

function showLoginPage() {
    closeRegisterPage();
    $.ajax({
        url: '/Index?handler=LoginPartial',
        type: 'GET',
        success: function (data) {
            $('#login-section').html(data);
            $('#login-section').show();

            $('#login-section').on('submit', '#login-form', function(event) {
                event.preventDefault();
                var form = $(this);
                $.ajax({
                    url: form.attr('action'),
                    type: 'POST',
                    data: form.serialize(),
                    success: function(result) {
                        console.log('AJAX success response:', result);

                        if (result.hasOwnProperty('success')) {
                            if (result.success) {
                                $('#login-success').html('Login successful! Redirecting...');
                                $('#login-error').html('');
                                form.trigger('reset');
                            } else {
                                $('#login-success').html('');
                                var errorMessage = result.errors ? result.errors.join('<br>') : "An unknown error occurred.";
                                $('#login-error').html(errorMessage);
                            }
                        } else {
                            console.error('Unexpected response format:', result);
                            $('#login-success').html('');
                            $('#login-error').html("An unknown error occurred.");
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        $('#login-success').html('');
                        $('#login-error').html("An error occurred during authentication.");
                    }
                });
            });
        }
    });
}

function showRegisterPage() {
    closeLoginPage();
    $.ajax({
        url: '/Index?handler=RegisterPartial',
        type: 'GET',
        success: function (data) {
            $('#register-section').html(data);
            $('#register-section').show();

            $('#register-section').on('submit', '#register-form', function(event) {
                event.preventDefault();
                var form = $(this);
                $.ajax({
                    url: form.attr('action'),
                    type: 'POST',
                    data: form.serialize(),
                    success: function(result) {
                        console.log('AJAX success response:', result);

                        if (result.hasOwnProperty('success')) {
                            if (result.success) {
                                $('#register-success').html('Registration successful! Please Login now.');
                                $('#register-error').html('');
                                form.trigger('reset');
                            } else {
                                $('#register-success').html('');
                                var errorMessage = result.errors ? result.errors.join('<br>') : "An unknown error occurred.";
                                $('#register-error').html(errorMessage);
                            }
                        } else {
                            console.error('Unexpected response format:', result);
                            $('#register-success').html('');
                            $('#register-error').html("An unknown error occurred.");
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        $('#register-success').html('');
                        $('#register-error').html("An error occurred during registration.");
                    }
                });
            });
        }
    });
}

function closeAboutPage() {
    $('#about-section').hide();
}

function closeContactPage() {
    $('#contact-section').hide();
}

function closeLoginPage() {
    $('#login-section').hide();
}

function closeRegisterPage() {
    $('#register-section').hide();
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