function navigateToPage(page, PageSize) {
    window.location.href = '/CurrentAuctions?CurrentPage=' + page + '&PageSize=' + PageSize;
}

function navigateToAuction(auctionId) {
    window.location.href = '/Auction?id=' + auctionId;
}