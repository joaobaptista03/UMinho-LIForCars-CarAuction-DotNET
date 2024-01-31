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

    var timeToEndAuction = document.querySelectorAll('.userLeiloes .timeToEndAuction')
    var currentTime = new Date();

    function checkIfEnding() {
        timeToEndAuction.forEach(function (timeLeftElement) {
            var endDateTimeString = timeLeftElement.dataset.enddatetime;
            var [day, month, year] = endDateTimeString.split('/');
            var endDateTime = new Date(`${month}/${day}/${year}`);
            currentTime = new Date();
            var timeLeft = (endDateTime - currentTime) / 1000;

            if (timeLeft<=0) {
                var auction = timeLeftElement.closest('.englobaAuction');
                auction.remove();
            } else if (timeLeft<=120) {
                timeLeftElement.textContent = Math.ceil(timeLeft) + ' seconds';
                timeLeftElement.style.display = 'block';
                if (timeLeftElement.style.color=='black') {
                    timeLeftElement.style.color='red'
                } else {
                    timeLeftElement.style.color='black'
                }
            }
        })

        currentTime = new Date();
        timeToEndAuction = document.querySelectorAll('.userLeiloes .timeToEndAuction')
    }

    setInterval(checkIfEnding, 800);

    var previousButton = document.getElementById('previousButton');

    if (previousButton != null) {
        previousButton.addEventListener('click', function () {
            window.location.href = '/Auctions?CurrentPage=' + (parseInt(previousButton.dataset.currentpage) - 1) + '&OrderBy=' + previousButton.dataset.orderby + (previousButton.dataset.filterby!="" ? '&FilterBy=' + previousButton.dataset.filterby : "");
        });
    }

    var nextButton = document.getElementById('nextButton');

    if (nextButton != null) {
        nextButton.addEventListener('click', function () {
            window.location.href = '/Auctions?CurrentPage=' + (parseInt(nextButton.dataset.currentpage) + 1) + '&OrderBy=' + nextButton.dataset.orderby + (nextButton.dataset.filterby!="" ? '&FilterBy=' + nextButton.dataset.filterby : "");
        });
    }

    var remainingTimeAscending = document.getElementById('remainingTimeAscending');

    if (remainingTimeAscending != null) {
        remainingTimeAscending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeAscending.dataset.currentpage)) + '&OrderBy=RemainingTimeAscending' + (remainingTimeAscending.dataset.filterby!="" ? '&FilterBy=' + remainingTimeAscending.dataset.filterby : "");
            }
        });
    }

    var remainingTimeDescending = document.getElementById('remainingTimeDescending');

    if (remainingTimeDescending != null) {
        remainingTimeDescending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeDescending.dataset.currentpage)) + '&OrderBy=RemainingTimeDescending' + (remainingTimeDescending.dataset.filterby!="" ? '&FilterBy=' + remainingTimeDescending.dataset.filterby : "");
            }
        });
    }
    
    var highestBidAscending = document.getElementById('highestBidAscending');

    if (highestBidAscending != null) {
        highestBidAscending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeAscending.dataset.currentpage)) + '&OrderBy=HighestBidAscending' + (highestBidAscending.dataset.filterby!="" ? '&FilterBy=' + highestBidAscending.dataset.filterby : "");
            }
        });
    }

    var highestBidDescending = document.getElementById('highestBidDescending');

    if (highestBidDescending != null) {
        highestBidDescending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(highestBidDescending.dataset.currentpage)) + '&OrderBy=HighestBidDescending' + (highestBidDescending.dataset.filterby!="" ? '&FilterBy=' + highestBidDescending.dataset.filterby : "");
            }
        });
    }

    document.getElementById('searchInput').addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault(); // Prevent form submission
            performSearch();
        }
    });

    function performSearch() {
        // Your JavaScript logic for handling the search
        var element = document.getElementById('searchInput');
        var searchTerm = element.value;

        resetElements = document.querySelectorAll('.englobaAuction');
        if (searchTerm!="") {
            var separarFiltro = searchTerm.split(":");

            // Verifica pelo quê que o user pretende procurar
            var inputValido = true;
            switch (capitalize(separarFiltro[0].trim())) {
                case "Make" || "Model" || "Year" || "Origin" || "Status":
                    window.location.href = '/Auctions?OrderBy=' + element.dataset.orderby +  "&FilterBy=" + searchTerm.trim();
                    break;
                case "Kms" || "Base price" || "Price":
                    var limites = separarFiltro[1].trim().split("-");
                    var min = limites[0].trim();
                    var max = limites[1].trim();
                    if (!isNaN(min) && !isNaN(max)) {
                        var intMin = parseInt(min);
                        var intMax = parseInt(max);
                        if (intMin<=intMax) {
                            window.location.href = '/Auctions?OrderBy=' + element.dataset.orderby +  "&FilterBy=" + searchTerm.trim();
                        } else {
                            inputValido = false;
                        }
                    } else {
                        inputValido = false;
                    }
                    break;
                default:
                    inputValido = false;
            }

            if (inputValido) {
                clearInput(true);
            } else {
                clearInput(false);
            }
        } else {
            if (element.dataset.filterby!="") {
                window.location.href = (window.location.href.split("&FilterBy="))[0];
            }
        }
    }

    function clearInput(inputValido) {
        // Clear the input field
        if (inputValido) {
            document.getElementById('searchInput').placeholder = 'Category, Motor ...';
            document.getElementById('searchInput').value = '';
        } else {
            document.getElementById('searchInput').placeholder = 'Invalid input';
            document.getElementById('searchInput').value = '';
        }
        
    }

    function capitalize(str) {
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    }
});


document.addEventListener('DOMContentLoaded', function () {
    const registerButton = document.getElementById('criar');
    const registerPage = document.getElementById('register-page');

    registerPage.style.display = 'none';

    // Event listener to toggle the visibility of the register page when the button is clicked
    registerButton.addEventListener('click', function () {
        toggleRegisterPage();
    });

    function toggleRegisterPage() {
        if (registerPage.style.display === 'none' || registerPage.style.display === '') {
            // If the register page is hidden or not set, show it
            registerPage.style.display = 'block';

            // Attach event listener for closing the register page
            document.addEventListener('click', closeRegisterPageOnClick);
        } else {
            // If the register page is visible, hide it
            registerPage.style.display = 'none';

            // Remove event listener for closing the register page
            document.removeEventListener('click', closeRegisterPageOnClick);
        }
    }

    // Function to close the register page
    function closeRegisterPage() {
        registerPage.style.display = 'none';

        // Remove event listener for closing the register page
        document.removeEventListener('click', closeRegisterPageOnClick);
    }

    // Event listener for closing the register page
    function closeRegisterPageOnClick(event) {
        if (!registerPage.contains(event.target) && event.target !== registerButton) {
            // If the click is outside the register page and not on the register button, close the register page
            closeRegisterPage();
        }
    }
});
