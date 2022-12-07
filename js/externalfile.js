/* A jquery function that imports a json file into a variable
 */

function randomNumberGenerator() {
    var randomNumber = Math.floor(Math.random() * 100);
    return randomNumber;
}

function importJsonObject() {
    var jsonObject = $.getJSON("js/data.json", function (data) {
        return data;
    });
    return jsonObject;
}