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

    function sortBidsDescending() {
        var bidsContainer = document.querySelector('.bidsContainer');
        var bids = Array.from(bidsContainer.children);

        bids.sort(function (a, b) {
            var bidValueA = parseFloat(a.dataset.bidvalue);
            var bidValueB = parseFloat(b.dataset.bidvalue);
            return bidValueB - bidValueA;
        });

        bids.forEach(function (bid) {
            bidsContainer.appendChild(bid);
        });
    }

    function sortBidsAscending() {
        var bidsContainer = document.querySelector('.bidsContainer');
        var bids = Array.from(bidsContainer.children);

        bids.sort(function (a, b) {
            var bidValueA = parseFloat(a.dataset.bidvalue);
            var bidValueB = parseFloat(b.dataset.bidvalue);
            return bidValueA - bidValueB;
        });

        bids.forEach(function (bid) {
            bidsContainer.appendChild(bid);
        });
    }

    function toggleSortOrder() {
        var triangleDown = document.querySelector('.triangle-down');

        if (triangleDown.classList.contains('ascending')) {
            // Sort in descending order
            sortBidsDescending();
            triangleDown.classList.remove('ascending');
        } else {
            // Sort in ascending order
            sortBidsAscending();
            triangleDown.classList.add('ascending');
        }
    }

    var sortButton = document.querySelector('.userLeiloes .additionalInfo .triangle-down');

    sortButton.addEventListener('click', function () {
        // Get the current rotation value (as a string)
        var currentRotation = this.style.transform.replace(/[^0-9]/g, '');

        // Toggle between 0 and 180 degrees
        var newRotation = currentRotation === '0' ? '180' : '0';

        // Apply the new rotation to the triangle
        this.style.transform = 'rotate(' + newRotation + 'deg)';

        toggleSortOrder();
    });

    $('#bid-form').submit(function (event) {
        event.preventDefault();

        var form = $(this);
        var bidValue = parseFloat($('#bid').val());
        console.log(bidValue);

        $.ajax({
            url: '/AuctionPage?handler=PlaceBid',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: form.serialize(),
            success: function (response) {
                if (response.hasOwnProperty('success')) {
                    if (response.success) {
                        $('#current-bid').html('Current Bid: ' + bidValue.toFixed(2) + ' €');
                        $('#bid-success').html('Bidded successfully!').show();
                        $('#bid-error').hide().html('');
                        form.trigger('reset');
                    } else {
                        $('#bid-success').hide().html('');
                        var errorMessage = response.errors ? response.errors.join('<br>') : "An unknown error occurred.";
                        $('#bid-error').html(errorMessage).show();
                    }
                } else {
                    $('#bid-success').hide().html('');
                    $('#bid-error').html("An unknown error occurred.").show();
                }
            },
            error: function (xhr, status, error) {
                $('#bid-success').hide().html('');
                $('#bid-error').html("An error occurred during bidding.").show();
            }
        });
    });

});

function showBidPage() {
    document.getElementById('bid-page').style.display = 'block';
}

function closeBidPage() {
    document.getElementById('bid-page').style.display = 'none';
}