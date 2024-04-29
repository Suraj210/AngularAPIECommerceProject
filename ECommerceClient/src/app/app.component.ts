import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from './services/ui/custom-toastr.service';
declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'ECommerceClient';

  ngOnInit(): void {
    // $.get('https://localhost:7188/api/products', (data) => {
    //   console.log(data);
    // });
  }

  // constructor(private toastrService: CustomToastrService) {
  //   toastrService.message('Hello', 'Suraj', {
  //     messageType: ToastrMessageType.Info,
  //     position: ToastrPosition.TopRight,
  //     closeButton:true,
  //   });
  //   toastrService.message('Hello', 'Suraj', {
  //     messageType: ToastrMessageType.Success,
  //     position: ToastrPosition.TopLeft,
  //     closeButton: true,
  //   });
  //   toastrService.message('Hello', 'Suraj', {
  //     messageType: ToastrMessageType.Warning,
  //     position: ToastrPosition.BottomCenter,
  //     closeButton: true,
  //   });
  //   toastrService.message('Hello', 'Suraj', {
  //     messageType: ToastrMessageType.Error,
  //     position: ToastrPosition.BottomRight,
  //     closeButton: true,
  //   });
  // }
}
