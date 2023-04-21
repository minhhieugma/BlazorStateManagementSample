// REF https://webinuse.com/how-to-warn-users-when-the-connection-is-lost/

/**
 * We can trigger onoffline event by using addEventListener
 * */
//
// window.addEventListener("offline", event => {
//     //here we run our code, what to do
//     //when connection is lost
//    
//     console.log("offline..")
// });


/**
 * We can trigger ononline event by using addEventListener
 * */
//
// window.addEventListener("online", event => {
//     //here we run our code, what to do
//     //when connection is restored
// });


/**
 * We can trigger onoffline event by using onoffline
 * */

window.onoffline = event => {
    //here we run our code, what to do
    //when connection is lost
    console.log("offline..");

    document.getElementById("lost-connection-modal").style.display = "block"
};


/**
 * We can trigger onoffline event by using ononline
 * */

window.ononline = event => {
    //here we run our code, what to do
    //when connection is restored

    console.log("online..");
    location.reload()
};