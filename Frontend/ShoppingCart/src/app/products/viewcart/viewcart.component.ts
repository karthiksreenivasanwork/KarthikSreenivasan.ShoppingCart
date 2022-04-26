import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-viewcart',
  templateUrl: './viewcart.component.html',
  styleUrls: ['./viewcart.component.css'],
})
export class ViewcartComponent implements OnInit {
  constructor(public productService: ProductsService) {}

  ngOnInit(): void {
    this.productService.getMyCartItems().subscribe((data)=>{
      console.log(data);
    });
    
  }
}
