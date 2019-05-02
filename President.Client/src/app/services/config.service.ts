import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {
     
    _apiURI : string;
    _hubURI : string;
 
    constructor() {
        this._apiURI = 'http://152.66.183.126:5000/api';
        this._hubURI = 'http://152.66.183.126:5000';
     }
 
     getApiURI() {
         return this._apiURI;
     }  

     getHubURI() {
         return this._hubURI;
     }  
}