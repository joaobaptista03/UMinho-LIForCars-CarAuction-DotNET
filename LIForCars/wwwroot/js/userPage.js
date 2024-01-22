document.addEventListener('DOMContentLoaded', function() {
    var homeLink = document.getElementById('homeLink');
    var aboutLink = document.getElementById('aboutLink');
    var contactLink = document.getElementById('contactLink');
    var leiloesLink = document.getElementById('leiloesLink');
    var bidsLink = document.getElementById('bidsLink');
    var leiloesContent = document.getElementById('leiloesContent'); // Replace with the actual ID of the element you want to show/hide
    var isLeiloesClicked = false;
    var bidsContent = document.getElementById('leiloesContent'); // Replace with the actual ID of the element you want to show/hide
    var isBidsClicked = false;


    function resetColorsCabecalho() {
        homeLink.style.color = '';
        aboutLink.style.color = '';
        contactLink.style.color = '';
    }

    homeLink.addEventListener('click', function() {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    aboutLink.addEventListener('click', function() {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    contactLink.addEventListener('click', function() {
        resetColorsCabecalho();
        this.style.color = '#d9534f';
    });

    function resetColorsUserBar() {
        leiloesLink.style.color = '';
        bidsLink.style.color = '';
    }

    leiloesLink.addEventListener('click', function() {
        if (!isLeiloesClicked) {
            resetColorsUserBar();
            this.style.color = '#d9534f';

            if (leiloesContent.style.display ==='none') {
                bidsContent.style.display = 'none';
                leiloesContent.style.display ='block';
            }

            isLeiloesClicked = true;
            isBidsClicked = false;
        }
        

    });

    bidsLink.addEventListener('click', function() {
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
});

