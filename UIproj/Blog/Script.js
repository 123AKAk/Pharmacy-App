function loaddata() {
    fetch('Blogurl.txt')
        .then(response => response.text())
        .then(data => {
            // Do something with your data
            var newUrl = data;
            if (data != "") {
                document.getElementById("scriturl").innerHTML = "You need to set your URL from the settings page";
            }
            else {
                window.location.href = newUrl;
            }
    });
}