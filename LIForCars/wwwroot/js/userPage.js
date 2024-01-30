document.addEventListener('DOMContentLoaded', function () {
    var homeLink = document.getElementById('homeLink');
    var aboutLink = document.getElementById('aboutLink');
    var contactLink = document.getElementById('contactLink');
    var leiloesLink = document.getElementById('leiloesLink');
    var bidsLink = document.getElementById('bidsLink');
    var waitingAuctionsLink = document.getElementById('waitingAuctionsLink');
    var statusLink = document.getElementById('statusLink');
    var leiloesContent = document.getElementById('leiloesContent'); // Replace with the actual ID of the element you want to show/hide
    var isLeiloesClicked = true;
    var bidsContent = document.getElementById('bidsContent'); // Replace with the actual ID of the element you want to show/hide
    var isBidsClicked = false;
    var statusContent = document.getElementById('statusContent'); // Replace with the actual ID of the element you want to show/hide
    var isStatusContentClicked = false;
    var waitingAuctionsContent = document.getElementById('waitingAuctionsContent'); // Replace with the actual ID of the element you want to show/hide
    var isWaitingAuctionsClicked = false;
    var highLightButtonLink = document.getElementById('higlLightButtonLink');
    var isHighLightButtonLink = false;
    var removeFinishedButtonLink = document.getElementById('removeFinishedButtonLink');
    var isRemoveFinishedButtonLink = false;

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
        waitingAuctionsLink.style.color = 'white';
        statusLink.style.color = 'white';
    }

    if (leiloesLink!= null) {
        leiloesLink.addEventListener('click', function () {
            if (!isLeiloesClicked) {
                resetColorsUserBar();
                this.style.color = '#d9534f';

                if (leiloesContent!=null && leiloesContent.style.display === 'none') {
                    bidsContent.style.display = 'none';
                    waitingAuctionsContent.style.display = 'none';
                    leiloesContent.style.display = 'block';
                    statusContent.style.display = 'none';
                }

                isLeiloesClicked = true;
                isBidsClicked = false;
                isWaitingAuctionsClicked = false;
                isStatusContentClicked = false;
            }
        });

        bidsLink.addEventListener('click', function () {
            if (!isBidsClicked) {
                resetColorsUserBar();
                this.style.color = '#d9534f';

                if (bidsContent!=null && bidsContent.style.display === 'none') {
                    leiloesContent.style.display = 'none';
                    waitingAuctionsContent.style.display = 'none';
                    bidsContent.style.display = 'block';
                    statusContent.style.display = 'none';
                }

                isBidsClicked = true;
                isLeiloesClicked = false;
                isWaitingAuctionsClicked = false;
                isStatusContentClicked = false;
            }
        });

        waitingAuctionsLink.addEventListener('click', function () {
            if (!isWaitingAuctionsClicked) {
                resetColorsUserBar();
                this.style.color = '#d9534f';

                if (waitingAuctionsContent!=null && waitingAuctionsContent.style.display === 'none') {
                    leiloesContent.style.display = 'none';
                    bidsContent.style.display = 'none';
                    waitingAuctionsContent.style.display = 'block';
                    statusContent.style.display = 'none';
                }

                isBidsClicked = false;
                isLeiloesClicked = false;
                isWaitingAuctionsClicked = true;
                isStatusContentClicked = false;
            }
        });

        statusLink.addEventListener('click', function () {
            if (!isStatusContentClicked) {
                resetColorsUserBar();
                this.style.color = '#d9534f';

                if (statusContent!=null && statusContent.style.display === 'none') {
                    leiloesContent.style.display = 'none';
                    bidsContent.style.display = 'none';
                    waitingAuctionsContent.style.display = 'none';
                    statusContent.style.display = 'block';
                }

                isBidsClicked = false;
                isLeiloesClicked = false;
                isWaitingAuctionsClicked = false;
                isStatusContentClicked = true;
            }
        });

        if (highLightButtonLink!=null) {
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
        }
        
        if (removeFinishedButtonLink!=null) {
            removeFinishedButtonLink.addEventListener('click', function () {
                if (!isRemoveFinishedButtonLink) {
                    this.style.backgroundColor = 'green';
                    this.style.borderColor = 'green';

                    var auctions = document.querySelectorAll("auctionInfoExtended");

                    auctions.forEach(function(auction) {
                        var endDateTimeString = auction.dataset.expired;
                        var [day, month, year] = endDateTimeString.split('/');
                        var endDateTime = new Date(`${month}/${day}/${year}`);
                        currentTime = new Date();
                        var timeLeft = (endDateTime - currentTime);

                        if (timeLeft<=0) {
                            auction.style.display='none';
                        }
                    })

                    isRemoveFinishedButtonLink = true;
                }
                else {
                    this.style.backgroundColor = '';
                    this.style.borderColor = '';

                    var auctions = document.querySelectorAll("auctionInfoExtended");

                    auctions.forEach(function(auction) {
                        auction.style.display='block';
                    })

                    isRemoveFinishedButtonLink = false;
                }
            });
        }
        

        var moreInfoButtons = document.querySelectorAll('.userLeiloes .moreInfoButton');

        moreInfoButtons.forEach(function (button) {
            button.addEventListener('click', function () {
                // Find the parent element with class 'auctionInfo'
                var auctionInfoDiv = this.closest('.englobaAuction');

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

        function sortBidsDescending(auctionId) {
            var bidsContainer = document.querySelector(`#${auctionId} .bidsContainer`);
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

        function sortBidsAscending(auctionId) {
            var bidsContainer = document.querySelector(`#${auctionId} .bidsContainer`);
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

        function toggleSortOrder(auctionId) {
            var triangleDown = document.querySelector(`#${auctionId} .triangle-down`);

            if (triangleDown.classList.contains('ascending')) {
                // Sort in descending order
                sortBidsDescending(auctionId);
                triangleDown.classList.remove('ascending');
            } else {
                // Sort in ascending order
                sortBidsAscending(auctionId);
                triangleDown.classList.add('ascending');
            }
        }

        function initializeSorting() {
            var sortButtons = document.querySelectorAll('.userLeiloes .additionalInfo .triangle-down');

            sortButtons.forEach(function (sortButton) {
                sortButton.addEventListener('click', function () {
                    // Get the current rotation value (as a string)
                    var currentRotation = this.style.transform.replace(/[^0-9]/g, '');

                    // Toggle between 0 and 180 degrees
                    var newRotation = currentRotation === '0' ? '180' : '0';

                    // Apply the new rotation to the triangle
                    this.style.transform = 'rotate(' + newRotation + 'deg)';

                    var auctionId = sortButton.getAttribute('data-auction-id');

                    toggleSortOrder(auctionId);
                });
            });
        }

        initializeSorting();
    }
    


});
