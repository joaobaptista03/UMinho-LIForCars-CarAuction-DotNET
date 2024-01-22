document.addEventListener('DOMContentLoaded', function () {
    const bidButton = document.querySelector('.bidbutton');
    const buyButton = document.querySelector('.buybutton');
    
    const bidPage = document.getElementById('bid-page');
    const buyPage = document.getElementById('buy-page');

    bidButton.addEventListener('click', function (e) {
        e.preventDefault();
        showBidPage();
    });

    buyButton.addEventListener('click', function (e) {
        e.preventDefault();
        showBuyPage();
    });


});

function showBidPage() {
    document.getElementById('bid-page').style.display = 'block';
}

function closeBidPage() {
    document.getElementById('bid-page').style.display = 'none';
}

function showBuyPage() {
    document.getElementById('buy-page').style.display = 'block';
}

function closeBuyPage() {
    document.getElementById('buy-page').style.display = 'none';
}
