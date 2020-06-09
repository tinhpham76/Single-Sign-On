import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-list-api-resource',
  templateUrl: './list-api-resource.component.html',
  styleUrls: ['./list-api-resource.component.scss']
})
export class ListApiResourceComponent implements OnInit {

  constructor(private translate: TranslateService) { }

  ngOnInit(): void {
  }

}
