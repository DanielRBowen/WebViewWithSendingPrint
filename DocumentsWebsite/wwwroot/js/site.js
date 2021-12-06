// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//const documentsHubConnection = new signalR.HubConnectionBuilder()
//    .withUrl(tenantPath + "/documentsHub?ClientId=" + '1')
//    .withAutomaticReconnect()
//    .configureLogging(signalR.LogLevel.None)
//    .build();

//async function start() {
//    try {
//        await documentsHubConnection.start()
//                .then(function () {
//                })
//                .catch(error => {
//                    console.log(error);
//                });

//        console.log("SignalR Connected.");
//    } catch (err) {
//        console.log(err);
//        setTimeout(start, 5000);
//    }
//};

//documentsHubConnection.onclose(async () => {
//    await start();
//});

//start();

if (document.getElementById("printDocumentButton") !== null) {
    $("#printDocumentButton").click(() => {
        let clientId = $("#ClientId").val();
        $.get("/documents/printdocument/" + clientId);
        //if (typeof documentsHubConnection !== 'undefined' && documentsHubConnection !== null) {
        //    try {
        //        await documentsHubConnection.invoke("printDocument", "", "Junk");
        //    } catch (err) {
        //        console.error(err);
        //    }
        //}
    });
}