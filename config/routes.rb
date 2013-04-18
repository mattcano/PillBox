PillBox::Application.routes.draw do

  resources :reminders


  resources :medications


  devise_for :users

  root :to => 'home#index'
  match "/mypillbox" => "pillbox#mypillbox"

end
