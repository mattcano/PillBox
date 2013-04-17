PillBox::Application.routes.draw do

  resources :medications


  devise_for :users

  root :to => 'home#index'

end
