import { Component, OnInit } from '@angular/core';

import { HomeDetails } from '../models/home.details.interface';
import { DashboardService } from '../services/dashboard.service';
import { PlayerStatistics } from '../models/player.statistics.interface';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  userName: string;
  stats: PlayerStatistics;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.userName = localStorage.getItem('user_name');
    this.dashboardService.getHomeDetails()
    .subscribe((stats: PlayerStatistics) => {
      this.stats = stats;
    });
  }
}