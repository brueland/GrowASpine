window.onload = function() {
    adjustPadding();
    window.addEventListener('resize', adjustPadding);
};

function adjustPadding() {
    var unityContainer = document.getElementById("unity-container");
    var gameInstructions = document.getElementById("game-instructions");

    // Get the height of the Unity container.
    var unityHeight = unityContainer.offsetHeight;

    // Set the padding-top of the instructions to be the same as the Unity container height.
    gameInstructions.style.paddingTop = unityHeight + "px";
}
