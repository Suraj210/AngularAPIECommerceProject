import { Component } from '@angular/core';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../../services/admin/alertify.service';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { SignalRService } from '../../../services/common/signalr.service';
import { ReceiveFunctions } from '../../../constants/receive-functions';
import { HubUrls } from '../../../constants/hub-url';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent extends BaseComponent {
  constructor(
    private alertify: AlertifyService,
    spinner: NgxSpinnerService,
    private signalRService: SignalRService
  ) {
    super(spinner);
    // signalRService.start(HubUrls.OrderHub);
    // signalRService.start(HubUrls.ProductHub);
  }
  ngOnInit(): void {
    this.signalRService.on(
      HubUrls.ProductHub,
      ReceiveFunctions.ProductAddedMessageReceiveFunction,
      (message) => {
        this.alertify.message(message, {
          messageType: MessageType.Notify,
          position: Position.TopRight,
        });
      }
    );
    this.signalRService.on(
      HubUrls.OrderHub,
      ReceiveFunctions.OrderAddedMessageReceiveFunction,
      (message) => {
        this.alertify.message(message, {
          messageType: MessageType.Notify,
          position: Position.TopCenter,
        });
      }
    );
  }

  m() {
    this.alertify.message('Merhaba', {
      messageType: MessageType.Success,
      delay: 5,
      position: Position.BottomCenter,
    });
  }

  d() {
    this.alertify.dismiss();
  }
}
