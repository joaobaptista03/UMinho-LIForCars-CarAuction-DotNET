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
    $.ajax({
        url: '/Index?handler=LoginPartial',
        type: 'GET',
        success: function (data) {
            $('#login-section').html(data);
            $('#login-section').show();

            // Attach event handler for form submission
            $('#login-form').submit(function(event) {
                event.preventDefault(); // Prevent default form submission
                var form = $(this);
                $.ajax({
                    url: form.attr('action'),
                    type: form.attr('method'),
                    data: form.serialize(),
                    success: function(result) {
                        if (result.success) {
                            console.log('Login successful');
                        } else {
                            console.log('Login failed');
                        }
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
                                $('#success-container').html('Registration successful!');
                                $('#error-container').html('');
                                form.trigger('reset');
                            } else {
                                $('#success-container').html('');
                                var errorMessage = result.errors ? result.errors.join('<br>') : "An unknown error occurred.";
                                $('#error-container').html(errorMessage);
                            }
                        } else {
                            console.error('Unexpected response format:', result);
                            $('#success-container').html('');
                            $('#error-container').html("An unknown error occurred.");
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error('AJAX error:', status, error);
                        $('#success-container').html('');
                        $('#error-container').html("An error occurred during registration.");
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