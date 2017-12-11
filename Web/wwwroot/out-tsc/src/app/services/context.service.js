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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Rx_1 = require("rxjs/Rx");
var app_context_1 = require("../models/app-context");
var ContextService = /** @class */ (function () {
    function ContextService() {
        //dashboard/market summary
        this.intervals = ['1h', '4h', '12h', '24h', '7d'];
        this.contextSource = new Rx_1.BehaviorSubject(new app_context_1.AppContext());
        this.context$ = this.contextSource.asObservable();
        this.context = new app_context_1.AppContext();
    }
    ContextService.prototype.setInterval = function (interval) {
        if (this.intervals.indexOf(interval) != -1) {
            this.context.interval = interval;
            this.contextSource.next(this.context);
        }
    };
    ContextService.prototype.setAssetCount = function (count) {
        if (count && count > 0) {
            this.context.assetCount = count;
            this.contextSource.next(this.context);
        }
    };
    ContextService.prototype.notify = function () {
        this.contextSource.next(this.context);
    };
    ContextService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ContextService);
    return ContextService;
}());
exports.ContextService = ContextService;
//# sourceMappingURL=context.service.js.map