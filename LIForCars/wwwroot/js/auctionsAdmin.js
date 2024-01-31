document.addEventListener('DOMContentLoaded', function () {
    var homeLink = document.getElementById('homeLink');
    var aboutLink = document.getElementById('aboutLink');
    var contactLink = document.getElementById('contactLink');

    function resetColorsCabecalho() {
        homeLink.style.color = '';
        aboutLink.style.color = '';
        contactLink.style.color = '';
    }

    homeLink.addEventListener('click', function () {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    aboutLink.addEventListener('click', function () {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    contactLink.addEventListener('click', function () {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    var previousButton = document.getElementById('previousButton');

    if (previousButton != null) {
        previousButton.addEventListener('click', function () {
            window.location.href = '/AuctionsAdmin?CurrentPage=' + (parseInt(previousButton.dataset.currentpage) - 1);
        });
    }

    var nextButton = document.getElementById('nextButton');

    if (nextButton != null) {
        nextButton.addEventListener('click', function () {
            window.location.href = '/AuctionsAdmin?CurrentPage=' + (parseInt(nextButton.dataset.currentpage) + 1);
        });
    }

    var refuseAuctionsButton = document.querySelectorAll('.userLeiloes .refuseButton')

    refuseAuctionsButton.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            var form = this.closest('form');
            deleteAuction(form);

            var auction = this.closest('.englobaAuction');
            auction.remove();
        });
    });

    var acceptAuctionsButton = document.querySelectorAll('.userLeiloes .acceptButton')

    acceptAuctionsButton.forEach(function (button) {
        button.addEventListener('click', function (event) {
            event.preventDefault();
            var form = this.closest('form');
            deleteAuction(form);

            var auction = this.closest('.englobaAuction');
            auction.remove();

            var totalAuctions = document.querySelectorAll('.userLeiloes .acceptButton')
            if (totalAuctions.length<2) {
                var footer = document.getElementById("fim");
                footer.style.position= 'fixed';
            }
        });
    });

    function deleteAuction(form) {
        $.ajax({
            url: form.getAttribute('action'),
            type: 'POST',
            data: new FormData(form),  // Use FormData to serialize the form data
            processData: false,
            contentType: false,
            success: function (response) {
                // Handle the success response, if needed
                console.log('Auction deleted successfully.');
            },
            error: function (error) {
                // Handle the error response, if needed
                console.error('Error deleting auction:', error);
            }
        });
    }
});
