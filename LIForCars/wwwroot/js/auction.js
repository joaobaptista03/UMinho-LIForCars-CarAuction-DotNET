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
    
        var form = $(this);
        var bidValue = parseFloat($('#bid').val());
        console.log(bidValue);

        $.ajax({
            url: '/Auction?handler=PlaceBid',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: form.serialize(),
            success: function(response) {
                if (response.hasOwnProperty('success')) {
                    if (response.success) {
                        $('#current-bid').html('Current Bid: ' + bidValue.toFixed(2));
                        $('#bid-success').html('Bidded successfuly!...');
                        $('#bid-error').html('');
                        form.trigger('reset');
                    } else {
                        $('#bid-success').html('');
                        var errorMessage = response.errors ? response.errors.join('<br>') : "An unknown error occurred.";
                        $('#bid-error').html(errorMessage);
                    }
                } else {
                    $('#bid-success').html('');
                    $('#bid-error').html("An unknown error occurred.");
        }
            },
            error: function(xhr, status, error) {
                $('#bid-success').html('');
                $('#bid-error').html("An error occurred during bidding.");
    }
        });
    });
});