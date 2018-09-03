import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {
     
    _apiURI : string;
 
    constructor() {
      //TODO chage this
        this._apiURI = 'https://localhost:5001/api';
     }
 
     getApiURI() {
         return this._apiURI;
     }    
}