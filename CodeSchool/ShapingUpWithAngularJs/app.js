(function() {
    var gems = [
      {
        name: 'Dodecahedron',
        price: 2.95,
        description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur scelerisque justo in justo sollicitudin fringilla. Praesent sed consequat enim. Donec eleifend pellentesque diam, sit amet rutrum orci finibus non. Phasellus semper ipsum dictum dignissim accumsan. Nunc semper eros vitae tempus malesuada. Morbi a odio massa. Curabitur eu nisl sit amet lacus dictum maximus sit amet id dolor. In maximus vehicula turpis, in condimentum velit sagittis ac.",
        canPurchase: true,
        soldOut: false,
        images:[
        {
          "full":"./img/dodecahedron-01-full.jpg",
          "thumb":"./img/dodecahedron-01-thumb.jpg"
        },
        {
          "full":"./img/dodecahedron-02-full.jpg",
          "thumb":"./img/dodecahedron-02-thumb.jpg"
        }]
      },
      {
        name: 'Pentagonal Gem',
        price: 5.95,
        description: "Sed interdum odio sit amet arcu cursus convallis. Donec sed nibh ac enim vestibulum semper. Duis in dui felis. Suspendisse a ullamcorper ex. Fusce congue nulla vel dui sagittis consectetur. Etiam semper, mauris ut cursus cursus, felis mi aliquet tellus, a sodales ligula lorem sit amet dolor. Mauris consequat mi id eleifend egestas. Maecenas mattis ipsum et purus mattis viverra. Vivamus a congue libero. Nunc nunc ipsum, lacinia ut fermentum sed, laoreet quis neque. Donec ante justo, aliquam vitae porttitor et, posuere eu enim. Sed enim sapien, condimentum nec viverra nec, dignissim non mauris. Fusce non eros in metus ultricies dapibus.",
        canPurchase: false,
        soldOut: false,
        images: [
        {
          "full":"./img/pentagonal-full.jpg",
          "thumb":"./img/pentagonal-thumb.jpg"
        }]
      }
    ];

    var app = angular.module('store', [ ]);

    app.controller('StoreController', function(){
      this.products = gems;
    });
})();
