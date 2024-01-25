$(document).ready(function() {
    $.ajax({
        url: '/Index?handler=HeaderPartial',
        type: 'GET',
        success: function(data) {
            $('#header-section').html(data);
            $('#header-section').show();
            $('#header-section').on('submit', '#logout-form', logout);
        }
    });
    $.ajax({
        url: '/Index?handler=FooterPartial',
        type: 'GET',
        success: function(data) {
            $('#footer-section').html(data);
            $('#footer-section').show();
        }
    });
});

function logout(event) {
    event.preventDefault();
    var form = $(this);
    var token = form.find('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function(result) {
            if (result.hasOwnProperty('success') && result.success) {
                window.location.reload();
            } else {
                console.error('Logout failed:', result);
            }
        },
        error: function(xhr, status, error) {
            console.error('Logout failed:', xhr, status, error);
        }
    });
}

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
                                $('#login-success').html('Login successful! Redirecting...').show();
                                $('#login-error').html('').hide();
                                form.trigger('reset');
                                setTimeout(function() {
                                    window.location.reload();
                                }, 1000);
                            } else {
                                $('#login-success').html('').hide();
                                var errorMessage = result.errors ? result.errors.join('<br>') : "An unknown error occurred.";
                                $('#login-error').html(errorMessage).show();
                            }
                        } else {
                            console.error('Unexpected response format:', result);
                            $('#login-success').html('').hide();
                            $('#login-error').html("An unknown error occurred.").show();
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        $('#login-success').html('').hide();
                        $('#login-error').html("An error occurred during authentication.").show();
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
                                $('#register-success').html('Registration successful! Please Login now.').show();
                                $('#register-error').html('').hide();
                                form.trigger('reset');
                            } else {
                                $('#register-success').html('').hide();
                                var errorMessage = result.errors ? result.errors.join('<br>') : "An unknown error occurred.";
                                $('#register-error').html(errorMessage).show();
                            }
                        } else {
                            console.error('Unexpected response format:', result);
                            $('#register-success').html('').hide();
                            $('#register-error').html("An unknown error occurred.").show();
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        $('#register-success').html('').hide();
                        $('#register-error').html("An error occurred during registration.").show();
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
            link.classList.add('active');
        }
    });
});