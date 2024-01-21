let page = 1;
const pageSize = 1;
let totalCount = 0;

async function loadAuctions() {
    const response = await fetch(`/api/Auction/current?page=${page}&pageSize=${pageSize}`);
    if (!response.ok) {
        console.error("Failed to load auctions.");
        return;
    }

    const auctions = await response.json();
    totalCount = parseInt(response.headers.get('X-Total-Count'), 10);
    const auctionsList = document.getElementById("auctionsList");

    auctionsList.innerHTML = ''; // Clear previous content

    if (auctions.length === 0) {
        auctionsList.innerHTML = '<div class="no-auctions">No current auctions available.</div>';
    } else {
        auctions.forEach(auction => {
            renderAuctionCard(auction);
        });
    }

    updatePagination();
}

function updatePagination() {
    document.getElementById("currentPage").textContent = `Page ${page}`;
    const maxPage = Math.ceil(totalCount / pageSize);

    document.getElementById("previousPageBtn").style.visibility = page > 1 ? 'visible' : 'hidden';
    document.getElementById("nextPageBtn").style.visibility = page < maxPage ? 'visible' : 'hidden';
}

function previousPage() {
    if (page > 1) {
        page--;
        loadAuctions();
    }
}

function nextPage() {
    if (page * pageSize < totalCount) {
        page++;
        loadAuctions();
    }
}

function renderAuctionCard(auction) {
    const card = document.createElement("div");
    card.className = "auction-card";
    card.innerHTML = `
        <div class="auction-info">
        <h3 class="car-model">${auction.car.make} ${auction.car.model} (${auction.car.launchYear})</h3>
            <p>Milage: ${auction.car.kms}</p>
            <p>Status Description: ${auction.car.statusDescription}</p>
            <p>Motor: ${auction.car.motor} (${auction.car.fuelType})</p>
            <p>Category: ${auction.car.category}</p>
            <p>Origin: ${auction.car.origin}</p>
            <p>Auction Dates: ${new Date(auction.initDateTime).toLocaleDateString()} - ${new Date(auction.endDateTime).toLocaleDateString()}</p>
            <p>Base Price: $${auction.basePrice}</p>
        </div>
    `;
    document.getElementById("auctionsList").appendChild(card);
}

function loadMore() {
    page++;
    loadAuctions();
}

// Initial load
document.addEventListener("DOMContentLoaded", loadAuctions);
