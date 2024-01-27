document.addEventListener('DOMContentLoaded', () => {

    const carAuctionForm = document.getElementById('carAuctionForm');

    if (carAuctionForm) {
        carAuctionForm.addEventListener('submit', async function (e) {
            e.preventDefault(); // Prevents the immediate form submission

            // Extract form data
            const formData = new FormData(carAuctionForm);

            const car = {
                Make: formData.get('Make'),
                Model: formData.get('Model'),
                LaunchYear: formData.get('LaunchYear'),
                Plate: formData.get('Plate'),
                Kms: formData.get('Kms'),
                Origin: formData.get('Origin'),
                Motor: formData.get('Motor'),
                FuelType: formData.get('FuelType'),
                StatusDescription: formData.get('StatusDescription'),
                Category: formData.get('Category'),
                CertificateNr: formData.get('CertificateNr'),

            };

                // Make POST request to create car
                const carResponse = await createCar(car);
                
                

                console.log('carResponse:', carResponse); // Log the entire response object


                const auction = {
                    BasePrice: formData.get('BasePrice'),
                    MinIncrement: formData.get('MinIncrement'),
                    StartDate: formData.get('InitDateTime'),
                    EndDate: formData.get('EndDateTime'),
                    BuyNowPrice: formData.get('BuyNowPrice'),
                    CarId: null,
    
                };

                // Extract the created car's ID
                const carId = (await carResponse.id);


                // Update auction with the carId
                auction.CarId = carId;

                // Make POST request to create auction
                const auctionResponse = await createAuction(auction);
                
        });
    }
});



// Inside the createCar function
async function createCar(car) {
    try {
        const response = await fetch('/api/Car/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: new URLSearchParams(car),
        });

        if (!response.ok) {
            const errorDetails = await response.json(); // Parse response body as JSON
            console.error('Error creating car:', errorDetails);
            throw new Error('Error creating car');
        }

        return response.json();
    } catch (error) {
        console.error('Error creating car or parsing response:', error);
        throw new Error('Error creating car');
    }
}


async function createAuction(auction) {
    try{
        const response = await fetch('/api/Auction/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded', // Change content type
            },
            body: new URLSearchParams(auction), // Use URLSearchParams to encode form data
        });
    
        if (!response.ok) {
            throw new Error('Error creating auction');
        }
        return response.json();

    } catch (error) {
        console.error('Error creating auction or parsing response:', error);
        throw new Error('Error creating auction');
    }

}




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

document.addEventListener('DOMContentLoaded', () => {
    const carForm = document.getElementById('carForm');
    if (carForm) {
        carForm.addEventListener('submit', async function(e) {
            e.preventDefault(); // Prevents the immediate form submission

            var make = document.getElementsByName('Make')[0].value;
            var model = document.getElementsByName('Model')[0].value;
            var launchYear = document.getElementsByName('LaunchYear')[0].value;
            var plate = document.getElementsByName('Plate')[0].value;
            var kms = document.getElementsByName('Kms')[0].value;
            var origin = document.getElementsByName('Origin')[0].value;
            var motor = document.getElementsByName('Motor')[0].value;
            var fuelType = document.getElementsByName('FuelType')[0].value;
            var statusDescription = document.getElementsByName('StatusDescription')[0].value;
            var category = document.getElementsByName('Category')[0].value;
            var certificateNr = document.getElementsByName('CertificateNr')[0].value;

            let errors = [];

            // Check if fields are empty
            if (!make) errors.push('Marca is required.');
            if (!model) errors.push('Modelo is required.');
            if (!launchYear) errors.push('Ano de Lançamento is required.');
            if (!plate) errors.push('Matrícula is required.');
            if (!kms) errors.push('Quilometragem is required.');
            if (!origin) errors.push('Origem do Veículo is required.');
            if (!motor) errors.push('Motor is required.');
            if (!fuelType) errors.push('Combustível is required.');
            if (!statusDescription) errors.push('Avaliação do Estado is required.');
            if (!category) errors.push('Tipo de Veículo is required.');
            if (!certificateNr) errors.push('Certificado is required.');

            // Perform uniqueness checks for kms and certificateNr in parallel
            const uniqueChecks = await Promise.all([
                isUnique('plate', plate),
                isUnique('certificateNr', certificateNr),
            ]);

            if (!uniqueChecks[0]) errors.push('Plate must be unique.');
            if (!uniqueChecks[1]) errors.push('Certificate must be unique.');

            if (errors.length > 0) {
                alert(errors.join('\n'));
                e.preventDefault();
                return;
            }

            carForm.submit(); // Submits the form if there are no errors
        });
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



async function fetchVehicleList() {
    try {
        const response = await fetch('/api/Cars/getAll');
        const vehicles = await response.json();
        
        const vehicleListContainer = document.getElementById('vehicle-list');

        if (vehicles && vehicles.length > 0) {
            const listHtml = '<h3>List of Vehicles:</h3><ul>' +
                vehicles.map(vehicle => `<li>${vehicle.marca} ${vehicle.modelo}</li>`).join('') +
                '</ul>';
            
            vehicleListContainer.innerHTML = listHtml;
        } else {
            vehicleListContainer.innerHTML = '<p>No vehicles found.</p>';
        }
    } catch (error) {
        console.error('Error fetching vehicle list:', error);
    }
}

function closeRegisterPage() {
    const registerPage = document.getElementById('register-page');
    registerPage.style.display = 'none'; // Oculta a página de registro
}
