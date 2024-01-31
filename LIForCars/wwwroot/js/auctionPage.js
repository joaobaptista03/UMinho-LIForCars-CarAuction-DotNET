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

// -------------------------------------- FORMS -------------------------------------- //

document.addEventListener('DOMContentLoaded', function () {
    const registerLink = document.querySelector('[data-target="register"]');
    
    // Elementos das páginas de registro 
    const registerPage = document.querySelector('#register-page'); // Substitua com o ID real da página de registro
    
    // Event listener para mostrar a página de registro quando o link de registro é clicado
    registerLink.addEventListener('click', function (e) {
        e.preventDefault(); // Evita o redirecionamento padrão
        hidePages(); // Oculta todas as páginas
        registerPage.style.display = 'block'; // Mostra a página de registro
    });
    
    
    // Função para ocultar todas as páginas
    function hidePages() {
        registerPage.style.display = 'none';
        loginPage.style.display = 'none';
        // Oculte outras páginas, se houver
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


async function isUnique(field, value) {
    try {
        const response = await fetch(`/api/car/checkUnique?field=${field}&value=${value}`);
        if (!response.ok) {
            throw new Error('Erro ao verificar a unicidade do campo');
        }
        return await response.json();
    } catch (error) {
        console.error('Erro na verificação de unicidade:', error);
        return false; // Considera não único em caso de erro
    }
}

document.addEventListener('click', function (event) {
    if (event.target.matches('.close-btn')) {
        // If the click target has the class 'close-btn', close the register page
        closeRegisterPage();
    }
});

function closeRegisterPage() {
    registerPage.style.display = 'none';
}



// -------------------------------------- Create BIDS -------------------------------------- //


async function createBid(bid) {
    try {
        const response = await fetch('/api/Bid/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(bid),
        });

        if (!response.ok) {
            //const errorDetails = await response.json();
            console.error('Error creating bid:', errorDetails);
            throw new Error('Error creating bid');
        }


        //return response.json();
    } catch (error) {
        console.error('Error creating bid or parsing response:', error);
        throw new Error('Error creating bid');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const bidForm = document.getElementById('bidForm');
    const bidButton = document.getElementById('este');

    if (bidForm && bidButton) {
        const auctionId = bidButton.dataset.auctionId || "";
        console.log('Auction ID:', auctionId);


        bidForm.addEventListener('submit', async function (e) {
            e.preventDefault();

            const formData = new FormData(bidForm);

            // Fetch additional details for the user
            const userResponse = await fetch(`/api/User/ById/${parseInt(formData.get('UserId'), 10)}`);
            const auctionResponse = await fetch(`/api/Auction/${parseInt(auctionId ,10)}`);

            if (!userResponse.ok || !auctionResponse.ok) {
                console.error('Error fetching additional details for user or auction');
                return;
            }

            let userData;
            try {
                userData = await userResponse.json();
            } catch (error) {
                console.error('Error parsing user data:', error);
                return;
            }
            const auctionData = await auctionResponse.json();


                // Fetch additional details for Car
            const carResponse = await fetch(`/api/Car/ById/${parseInt(auctionData.carId,10)}`);
            let carData = null;
            if (carResponse.ok) {
                try {
                    carData = await carResponse.json();
                } catch (error) {
                    console.error('Error parsing car data:', error);
                }
            }

            // Fetch additional details for Administrator
            const adminResponse = await fetch(`/api/Administrator/ById/${parseInt(auctionData.administratorId,10)}`);
            let adminData = null;
            if (adminResponse.ok) {
                try {
                    adminData = await adminResponse.json();
                } catch (error) {
                    console.error('Error parsing administrator data:', error);
                }
            }

            const auctionUserResponse = await fetch(`/api/User/ById/${parseInt(auctionData.userId,10)}`);
            let auctionUserData = null;
            if (auctionUserResponse.ok) {
                try {
                    auctionUserData = await auctionUserResponse.json();
                } catch (error) {
                    console.error('Error parsing administrator data:', error);
                }
            }


            // Create the bid object after fetching user and auction data
            const bid = {
                'AuctionId': parseInt(auctionId, 10),
                'UserId': parseInt(formData.get('UserId'), 10),
                'BidValue': formData.get('BidValue'),
                'bidTime': formData.get('bidTime'),
                'User': {
                    //'Id': userData.id,
                    'Name': userData.name,
                    'Nif': userData.nif,
                    'CC': userData.cc,
                    'Address': userData.address,
                    'Phone': userData.phone,
                    'Gender': userData.gender,
                    'BirthDate': userData.birthDate,
                    'Username': userData.username,
                    'Email': userData.email,
                    'Password': userData.password,
                    'IsAuctioneer': userData.isAuctioneer,
                },
                'Auction': {
                    //'Id': auctionData.id,
                    'BasePrice': auctionData.basePrice,
                    'MinIncrement': auctionData.minIncrement,
                    'InitDateTime': auctionData.initDateTime,
                    'EndDateTime': auctionData.endDateTime,
                    'BuyNowPrice': auctionData.buyNowPrice,
                    'CarId': auctionData.carId,
                    'Car': {
                        //'Id': carData.id,
                        'Make': carData.make,
                        'LaunchYear': carData.launchYear,
                        'Plate': carData.plate,
                        'Kms': carData.kms,
                        'StatusDescription': carData.statusDescription,
                        'Motor': carData.motor,
                        'FuelType': carData.fuelType,
                        'Category': carData.category,
                        'Origin': carData.origin,
                        'CertificateNr': carData.certificateNr,
                        'PicUrl': carData.picUrl,
                    },
                    'AdministratorId': auctionData.administratorId,
                    'Administrator': {
                        //'Id': adminData.id,
                        'Name': adminData.name,
                        'Nif': adminData.nif,
                        'CC': adminData.cc,
                        'Address': adminData.address,
                        'Phone': adminData.phone,
                        'Gender': adminData.gender,
                        'BirthDate': adminData.birthDate,
                        'Username': adminData.username,
                        'Email': adminData.email,
                        'Password': adminData.password,
                        'IsAuctioneer': adminData.isAuctioneer,
                        'ContractNr': adminData.contractNr,
                    },
                    'UserId': auctionData.userId,
                    'User': {
                        //'Id': auctionUserResponse.id,
                        'Name': auctionUserResponse.name,
                        'Nif': auctionUserResponse.nif,
                        'CC': auctionUserResponse.cc,
                        'Address': auctionUserResponse.address,
                        'Phone': auctionUserResponse.phone,
                        'Gender': auctionUserResponse.gender,
                        'BirthDate': auctionUserResponse.birthDate,
                        'Username': auctionUserResponse.username,
                        'Email': auctionUserResponse.email,
                        'Password': auctionUserResponse.password,
                        'IsAuctioneer': auctionUserResponse.isAuctioneer,
                    },
                    'Autorized': auctionData.autorized,
                },
            };
            

            try {
                console.log('Bid Object:', bid);

                const bidResponse = await createBid(bid);
                //console.log('bidResponse:', bidResponse);
                // Add logic to handle the bid response as needed
            } catch (error) {
                console.error('Error in createBid:', error);
            }
        });
    }
});
