import { Component } from '@angular/core';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClientService } from '../../../services/common/http-client.service';
import { Product } from '../../../contracts/product';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent extends BaseComponent {
  constructor(
    spinner: NgxSpinnerService,
    private httpClientService: HttpClientService
  ) {
    super(spinner);
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BallAtom);

    this.httpClientService
      .get<Product[]>({
        controller: 'products',
      })
      .subscribe((data) => {
        console.log(data);
      });

    // this.httpClientService
    //   .post(
    //     {
    //       controller: 'products',
    //     },
    //     {
    //       name: 'Kalem',
    //       stock: 100,
    //       price: 15,
    //     }
    //   )
    //   .subscribe();

    // this.httpClientService
    //   .put(
    //     {
    //       controller: 'products',
    //     },
    //     {
    //       id: 'a9ec79c6-4627-40c7-b2be-490c9a95697e',
    //       name: 'Dry Paper',
    //       stock: 111,
    //       price: 5.5,
    //     }
    //   )
    //   .subscribe();

    // this.httpClientService
    //   .delete(
    //     { controller: 'products' },
    //     'a9ec79c6-4627-40c7-b2be-490c9a95697e'
    //   )
    //   .subscribe();

    // this.httpClientService
    //   .get({
    //     fullEndPoint: 'https://jsonplaceholder.typicode.com/posts',
    //   })
    //   .subscribe((data) => console.log(data));
  }
}
