//import { Component, Inject } from '@angular/core';
//import { HttpClient } from '@angular/common/http';

//@Component({
//  selector: 'shouts',
//  templateUrl: './shout.component.html'
//})
//export class ShoutsComponent {
//  public shouts: Shout[] = [];

//  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
//    http.get<Shout[]>(`${baseUrl}api/shout`).subscribe(result => {
//      this.shouts = result;
//    }, error => console.error(error));
//  }
//}

//interface Shout {
//  username: string;
//  message: string;
  
//}



////var ClientApp = angular.module('shoutyApp', []);

////shoutyApp.controller('ShoutController', function ($scope) {
////  var size = 5;
////  $scope.currentSize = size;
////  $scope.user = 'Anonymous';
////  $scope.shouts = [{
////    'user': 'Gungor Budak',
////    'message': 'Hello world'
////  }];
////  $scope.shout = function () {
////    if ($scope.message.length > 0) {
////      $scope.shouts.unshift({
////        'user': $scope.user,
////        'message': $scope.message
////      });
////      $scope.message = '';
////      $scope.currentSize = size;
////    }
////  };
////  $scope.showMore = function () {
////    $scope.currentSize += size;
////  };
////});
