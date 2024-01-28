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

});
