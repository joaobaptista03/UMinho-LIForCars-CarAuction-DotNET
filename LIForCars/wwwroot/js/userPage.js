document.addEventListener('DOMContentLoaded', function () {
    var homeLink = document.getElementById('homeLink');
    var aboutLink = document.getElementById('aboutLink');
    var contactLink = document.getElementById('contactLink');
    var leiloesLink = document.getElementById('leiloesLink');
    var bidsLink = document.getElementById('bidsLink');
    var leiloesContent = document.getElementById('leiloesContent'); // Replace with the actual ID of the element you want to show/hide
    var isLeiloesClicked = true;
    var bidsContent = document.getElementById('bidsContent'); // Replace with the actual ID of the element you want to show/hide
    var isBidsClicked = false;
    var highLightButtonLink = document.getElementById('higlLightButtonLink');
    var isHighLightButtonLink = false;

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

    function resetColorsUserBar() {
        leiloesLink.style.color = 'white';
        bidsLink.style.color = 'white';
    }

    leiloesLink.addEventListener('click', function () {
        if (!isLeiloesClicked) {
            resetColorsUserBar();
            this.style.color = '#d9534f';

            if (leiloesContent.style.display === 'none') {
                bidsContent.style.display = 'none';
                leiloesContent.style.display = 'block';
            }

            isLeiloesClicked = true;
            isBidsClicked = false;
        }
    });

    bidsLink.addEventListener('click', function () {
        if (!isBidsClicked) {
            resetColorsUserBar();
            this.style.color = '#d9534f';

            if (bidsContent.style.display === 'none') {
                leiloesContent.style.display = 'none';
                bidsContent.style.display = 'block';
            }

            isBidsClicked = true;
            isLeiloesClicked = false;
        }
    });

    highLightButtonLink.addEventListener('click', function () {
        if (!isHighLightButtonLink) {
            this.style.backgroundColor = 'green';
            this.style.borderColor = 'green';

            var elementsWithClass = document.getElementsByClassName("infoMyBids");

            for (var i = 0; i < elementsWithClass.length; i++) {
                elementsWithClass[i].style.backgroundColor = "green";
            }

            isHighLightButtonLink = true;
        }
        else {
            this.style.backgroundColor = '';
            this.style.borderColor = '';

            var elementsWithClass = document.getElementsByClassName("infoMyBids");

            for (var i = 0; i < elementsWithClass.length; i++) {
                elementsWithClass[i].style.backgroundColor = "white";
            }

            isHighLightButtonLink = false;
        }
    });

    var moreInfoButtons = document.querySelectorAll('.userLeiloes .auctionInfo button');

    moreInfoButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Find the parent element with class 'auctionInfo'
            var auctionInfoDiv = this.closest('.auctionInfoExtended');

            // Find the '.additionalInfo' element within the 'auctionInfo' div
            var infoDiv = auctionInfoDiv.querySelector('.additionalInfo');

            // Toggle visibility of the '.additionalInfo' element
            toggleVisibility(infoDiv);
        });
    });

    function toggleVisibility(element) {
        if (element) {
            if (element.style.display === 'none' || element.style.display === '') {
                element.style.display = 'block';
            } else {
                element.style.display = 'none';
            }
        }
    }

    var sortButtons = document.querySelectorAll('.userLeiloes .additionalInfo .triangle-down');

    sortButtons.forEach(function (sortButton) {
        sortButton.addEventListener('click', function () {
            // Get the current rotation value (as a string)
            var currentRotation = this.style.transform.replace(/[^0-9]/g, '');

            // Toggle between 0 and 180 degrees
            var newRotation = currentRotation === '0' ? '180' : '0';

            // Apply the new rotation to the triangle
            this.style.transform = 'rotate(' + newRotation + 'deg)';

            // Find the parent element with class 'auctionInfo'
            var auctionInfoDiv = this.closest('.additionalInfoBox');

            // Find the '.additionalInfo' element within the 'auctionInfo' div
            var descendingBids = auctionInfoDiv.querySelector('.sortBidsDescending');
            var ascendingBids = auctionInfoDiv.querySelector('.sortBidsAscending');

            // Toggle visibility of the '.additionalInfo' element
            toggleAscending(descendingBids, ascendingBids);
        });
    });

    function toggleAscending(descendingBids, ascendingBids) {
        if (descendingBids && ascendingBids) {
            if (descendingBids.style.display === 'none' || descendingBids.style.display === '') {
                ascendingBids.style.display = 'none';
                descendingBids.style.display = 'block';
            } else {
                descendingBids.style.display = 'none';
                ascendingBids.style.display = 'block';
            }
        }
    }
});
