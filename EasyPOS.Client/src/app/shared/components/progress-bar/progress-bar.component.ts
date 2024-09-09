import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.scss'
})
export class ProgressBarComponent {
  @Input() value: number = null;
  @Input() showValue: boolean = true;
  @Input() styleClass: string = null;
  @Input() style: any = null;
  @Input() unit: string = '%';
  @Input() mode: string = "determinate";
  @Input() color: string = null;
  

}
