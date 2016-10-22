"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var CarPartsComponent = (function () {
    function CarPartsComponent() {
        this.carParts = [{
                "id": 1,
                "name": "Super Tires",
                "description": "These tires are the very best",
                "inStock": 5,
                "price": 4.99
            },
            {
                "id": 2,
                "name": "Super Tires 2",
                "description": "These tires are the very best of the best",
                "inStock": 3,
                "price": 7.99
            },
            {
                "id": 3,
                "name": "Super Tires 5",
                "description": "These tires are the very best of the best",
                "inStock": 0,
                "price": 9.99
            }];
    }
    CarPartsComponent.prototype.totalCarParts = function () {
        // let sum = 0;
        //
        // for(let carPart of this.carParts){
        //   sum += carPart.inStock;
        // }
        //
        // return sum;
        return this.carParts.reduce(function (prev, current) { return prev + current.inStock; }, 0);
    };
    ;
    return CarPartsComponent;
}());
CarPartsComponent = __decorate([
    core_1.Component({
        selector: 'car-parts',
        templateUrl: 'app/car-parts.component.html',
        styleUrls: ['app/car-parts.component.css']
    }),
    __metadata("design:paramtypes", [])
], CarPartsComponent);
exports.CarPartsComponent = CarPartsComponent;
//# sourceMappingURL=car-parts.component.js.map