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
            window.location.href = '/Auctions?CurrentPage=' + (parseInt(previousButton.dataset.currentpage) - 1);
        });
    }

    var nextButton = document.getElementById('nextButton');

    if (nextButton != null) {
        nextButton.addEventListener('click', function () {
            window.location.href = '/Auctions?CurrentPage=' + (parseInt(nextButton.dataset.currentpage) + 1);
        });
    }

    var remainingTimeAscending = document.getElementById('remainingTimeAscending');

    if (remainingTimeAscending != null) {
        remainingTimeAscending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeAscending.dataset.currentpage)) + '&OrderBy=RemainingTimeAscending';
            }
        });
    }

    var remainingTimeDescending = document.getElementById('remainingTimeDescending');

    if (remainingTimeDescending != null) {
        remainingTimeDescending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeDescending.dataset.currentpage)) + '&OrderBy=RemainingTimeDescending';
            }
        });
    }
    
    var highestBidAscending = document.getElementById('highestBidAscending');

    if (highestBidAscending != null) {
        highestBidAscending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(remainingTimeAscending.dataset.currentpage)) + '&OrderBy=HighestBidAscending';
            }
        });
    }

    var highestBidDescending = document.getElementById('highestBidDescending');

    if (highestBidDescending != null) {
        highestBidDescending.addEventListener('click', function () {

            if (this.style.borderBottom!="5px solid white") {
                window.location.href = '/Auctions?CurrentPage=' + (parseInt(highestBidDescending.dataset.currentpage)) + '&OrderBy=HighestBidDescending';
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
        var searchTerm = document.getElementById('searchInput').value;
        console.log('Performing search for: ' + searchTerm);

        resetElements = document.querySelectorAll('.englobaAuction');
        if (searchTerm!="") {
            resetElements.forEach(a => a.style.display = 'none');

            var separarFiltro = searchTerm.split(":");

            // Verifica pelo quê que o user pretende procurar
            var filtrados = null;
            var inputValido = true;
            switch (capitalize(separarFiltro[0].trim())) {
                case "Make":
                    var filtro = separarFiltro[1].trim().replace(/\s+/g, '_');
                    filtrados = document.querySelectorAll('.Make' + filtro);
                    break;
                case "Model":
                    var filtro = separarFiltro[1].trim().replace(/\s+/g, '_');
                    filtrados = document.querySelectorAll('.Model' + filtro);
                    break;
                case "Year":
                    var filtro = separarFiltro[1].trim();
                    filtrados = document.querySelectorAll('.Year' + filtro);
                    break;
                case "Origin":
                    var filtro = separarFiltro[1].trim().replace(/\s+/g, '_');
                    filtrados = document.querySelectorAll('.Origin' + filtro);
                    break;
                case "Motor":
                    var filtro = separarFiltro[1].trim().replace(/\s+/g, '_');
                    filtrados = document.querySelectorAll('.Motor' + filtro);
                    break;
                case "Kms":
                    var limites = separarFiltro[1].trim().split("-");
                    var min = limites[0].trim();
                    var max = limites[1].trim();
                    var intMin = parseInt(min);
                    var intMax = parseInt(max);
                    if (!isNaN(intMin) && !isNaN(intMax) && intMin<=intMax) {
                        var filtradosAux = [];
                        for (let i = intMin; i<=intMax; i++) {
                            filtradosAux = [...filtradosAux, ...document.querySelectorAll('.Kms' + i + "_00")];
                        }
                        filtrados = filtradosAux;
                    }
                    break;
                case "Status":
                    var filtro = separarFiltro[1].trim().replace(/\s+/g, '_');
                    filtrados = document.querySelectorAll('.Status' + filtro);
                    break;
                case "Base price":
                    var limites = separarFiltro[1].trim().split("-");
                    var min = limites[0].trim();
                    var max = limites[1].trim();
                    var intMin = parseInt(min);
                    var intMax = parseInt(max);
                    if (!isNaN(intMin) && !isNaN(intMax) && intMin<=intMax) {
                        var filtradosAux = [];
                        for (let i = intMin; i<=intMax; i++) {
                            filtradosAux = [...filtradosAux, ...document.querySelectorAll('.Base_Price' + i + "_00")];
                        }
                        filtrados = filtradosAux;
                    }
                    break;
                case "Price":
                    var limites = separarFiltro[1].trim().split("-");
                    var min = limites[0].trim();
                    var max = limites[1].trim();
                    var intMin = parseInt(min);
                    var intMax = parseInt(max);
                    if (!isNaN(intMin) && !isNaN(intMax) && intMin<=intMax) {
                        var filtradosAux = [];
                        for (let i = intMin; i<=intMax; i++) {
                            filtradosAux = [...filtradosAux, ... document.querySelectorAll('.Price' + i + "_00")];
                        }
                        filtrados = filtradosAux;
                    }
                    break;
                default:
                    inputValido = false;
                    resetElements.forEach(a => a.style.display = 'block');
                    clearInput(false);
            }

            if (inputValido) {
                filtrados.forEach(a => {
                    a.style.display = 'block';
                });
                clearInput(true);
            }
        } else {
            resetElements.forEach(a => {
                a.style.display = 'block';
            });
            clearInput(true);
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
