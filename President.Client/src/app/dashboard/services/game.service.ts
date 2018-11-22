import { Injectable } from '@angular/core';
import { BaseService } from '../../services/base.service';
import { Http, Headers } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { Card } from '../models/card.interface';

@Injectable()
export class GameService extends BaseService {
  baseUrl: string = '';

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.baseUrl = configService.getApiURI();
  }

  private getHeader(): Headers{
    let headers = new Headers();
    headers.append('Content-Type', 'text/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return headers;
  }

  public sendCard(card: Card) {
    let headers =  this.getHeader();

    return this.http
      .post(this.configService.getApiURI() + "/game/card", card, {headers})
      .catch(this.handleError);
  }

  public pass(){
    let headers = this.getHeader();

    return this.http
      .post(this.configService.getApiURI() + "/game/pass", null, {headers})
      .catch(this.handleError);
  }

  public switchCards(cards: Array<Card>){
    let headers =  this.getHeader();

    return this.http
      .post(this.configService.getApiURI() + "/game/switch", cards, {headers})
      .catch(this.handleError);
  }
}
