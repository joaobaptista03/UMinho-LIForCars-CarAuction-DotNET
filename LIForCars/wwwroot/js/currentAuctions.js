let page = 1;
const pageSize = 10;

async function loadAuctions() {
    const response = await fetch(`/api/Auction/current?page=${page}&pageSize=${pageSize}`);
    if (!response.ok) {
        console.error("Failed to load auctions.");
        return;
    }

    const auctions = await response.json();
    const totalCount = response.headers.get('X-Total-Count');
    const auctionsList = document.getElementById("auctionsList");

    // Check if the first page is being loaded and clear previous content
    if (page === 1) {
        auctionsList.innerHTML = '';
    }

    // Display a message if there are no auctions
    if (auctions.length === 0 && page === 1) {
        auctionsList.innerHTML = '<div class="no-auctions">No current auctions available.</div>';
        document.querySelector(".load-more-btn").style.display = 'none'; // Hide the Load More button if no auctions
        return; // Exit the function early
    }

    auctions.forEach(auction => {
        renderAuctionCard(auction);
    });

    if ((page * pageSize) >= totalCount) {
        document.querySelector(".load-more-btn").style.display = 'none';
    }
}

function renderAuctionCard(auction) {
    const card = document.createElement("div");
    card.className = "auction-card";
    card.innerHTML = `
        <img src="${auction.thumbnailUrl || 'default_thumbnail_path_if_any'}" alt="Car Image" class="auction-img">
        <div class="auction-info">
            <h3 class="car-model">${auction.carModel}</h3>
            <p class="auction-dates">Auction Dates: ${new Date(auction.start).toLocaleDateString()} - ${new Date(auction.end).toLocaleDateString()}</p>
            <p class="base-price">Base Price: $${auction.basePrice}</p>
            <button class="bid-btn" onclick="placeBid(${auction.id})">Place a Bid</button>
        </div>
    `;
    document.getElementById("auctionsList").appendChild(card);
}

function loadMore() {
    page++;
    loadAuctions();
}

// Placeholder for the placeBid function
function placeBid(auctionId) {
    console.log("Bid for auction ID:", auctionId);
    // Implement the function to handle bid placement
}

// Initial load
document.addEventListener("DOMContentLoaded", loadAuctions);
