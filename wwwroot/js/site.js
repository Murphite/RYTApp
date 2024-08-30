// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let viewProfileBtn = document.querySelector("#view-profile-btn");
let viewLogoutBtn = document.querySelector("#view-logout-btn");
let noLogoutBtn = document.querySelector("#no-logout-btn");

let usernameCols = document.querySelectorAll(".username-col");
let userIdInput = document.querySelector("#userid-input");
let amtToSendInput = document.querySelector("#amt-to-send-input");

let sendBtn = document.querySelector("#send-rwd-btn");

usernameCols.forEach(col => {
    col.addEventListener('click', () => {
        userIdInput.value = col.name;
        $("#sendReward").modal("show")
    })
})

viewProfileBtn.addEventListener('click', () => {
    $("#account_settings").modal("hide")
    $("#profile").modal("show")
})

viewLogoutBtn.addEventListener('click', () => {
    $("#account_settings").modal("hide")
    $("#logout").modal("show")
})

noLogoutBtn.addEventListener('click', () => {
    $("#logout").modal("hide")

})

sendBtn.addEventListener("click", () => {
    fetch(`SendReward?userId=${userIdInput.value}&amount=${amtToSendInput.value}`,
        {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }
    ).then(res => {
        // Check if the response status is OK (200)
        if (!res.ok) {
            throw new Error(`HTTP error! Status: ${res.status}`);
        }

        // Parse the JSON response
        return res.json();
    })
        .then(data => {
            // Handle the result from the controller
            console.log(data);

            let output = ''; 
            if (data.code == "200") {
                output = `
               
                <div class="ryts-2-font">
                 
                    <p class="ryts-2-p1">Money is on the way</p>
                    <div class="ryts-2-text-wrap">
                        <p class="ryts-2-p2">
                            You have successfully sent ${data.amount}<br>
                            to <span class="ryts-2-patience">${data.name}.</span> Cheers
                        </p>
                        
                    </div>
                </div>
            `;
            }

            else if (data.code == "600") {
                output = `
                <div>
                <p> ${data.invalidEntryResult} </p>
                </div>
                `;
            }
            else if (data.code == "300") {
                output = `
                <div>
                <p> ${data.insufficientBalanceResult} </p>
                </div>
                `;
            }
            else if (data.code == "404") {
                output = `
                <div>
                <p> ${data.userNotFoundResult}</p>
                </div>
                `;

            }
            else if (data.code == "700") {
                output = `
                <div>  
                <p> ${data.inactiveWalletResult}</p>
                </div>
                `;
            }
            else if (data.code = "400") {
                output = `
                   <div>
                   <p>${data.unsuccesfulTransactionResult}</p>
                   </div>
                `;
            }
            else if (data.code == "800")
            {
                output = `
                <div>
                <p>${data.noAmountResult} </p>
                </div>
                `;
            }
            // Example: Update content of a new modal with the result
            document.getElementById('resultModalContent').innerHTML = output;
           
            $("#sendReward").modal("hide")
            $('#sendRewardSuccess').modal('show');
        })
        .catch(error => {
            console.error('Error occurred while sending data:', error);
        });
});