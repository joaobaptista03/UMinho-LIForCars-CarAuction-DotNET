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

$(document).ready(function() {
    $('#bid-form').submit(function(event) {
        event.preventDefault();
        var auctionId = $('#auction-data').data('auction-id');

        var form = $(this);
        $.ajax({
            url: '/Auction?handler=PlaceBid',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: form.serialize(),
            success: function(response) {
                console.log('AJAX success response:', response);

                if (response.hasOwnProperty('success')) {
                    if (response.success) {
                        alert("Bid placed successfully!");
                        form.trigger('reset');
                        closeBidPage();
                    } else {
                        var errorMessage = response.errorMessage || "Failed to place bid.";
                        alert(errorMessage);
                    }
                } else {
                    console.error('Unexpected response format:', response);
                    alert("An unexpected error occurred.");
                }
            },
            error: function(xhr, status, error) {
                console.error('AJAX error:', status, error);
                alert("An error occurred while placing the bid.");
            }
        });
    });
});