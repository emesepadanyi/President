import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ChatComponent } from './chat.component';
import { FormsModule } from '@angular/forms';
import { Http } from '@angular/http';
import { ConfigService } from '../../services/config.service';
import { of } from 'rxjs/observable/of';

describe('ChatComponent', () => {
  let component: ChatComponent;
  let fixture: ComponentFixture<ChatComponent>;

  beforeEach(async(() => {
    const httpService = jasmine.createSpyObj('Http', ['post']);
    httpService.post.and.returnValue(of(Request));

    TestBed.configureTestingModule({
      declarations: [
        ChatComponent
      ]
      , providers: [
        ConfigService,
        { provide: Http, useValue: httpService }
      ]
      , imports: [
        FormsModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});
