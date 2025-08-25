import { Component, Input } from '@angular/core';
import { JsonModel } from '../../send-json.models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-send-json-list-view',
  imports: [
    CommonModule
  ],
  templateUrl: './send-json-list-view.component.html',
  styleUrl: './send-json-list-view.component.scss'
})
export class SendJsonListViewComponent {
  @Input() loading: boolean | null = false;
  @Input() error: string | null = null;
  @Input() items: JsonModel[] | null = [];
}
