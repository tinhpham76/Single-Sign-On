import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.scss']
})
export class AccessDeniedComponent implements OnInit {

  constructor(private translate: TranslateService,
    private location: Location) { }

  ngOnInit() {
  }
  goBack() {
    this.location.back();
  }
}
