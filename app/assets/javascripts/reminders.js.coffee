# Place all the behaviors and hooks related to the matching controller here.
# All this logic will automatically be available in application.js.
# You can use CoffeeScript in this file: http://jashkenas.github.com/coffee-script/

$(document).ready ->
  $(".email_reminder").on "click", (event) ->
  link = $(this)
  event.preventDefault()
  $.ajax
    url: link.attr("href")
    method: "get"
    dataType: "json"
    beforeSend: ->

    complete: ->

    success: ->

    error: ->

  $(".text_reminder").on "click", (event) ->
  link = $(this)
  event.preventDefault()
  $.ajax
    url: link.attr("href")
    method: "get"
    dataType: "json"
    beforeSend: ->

    complete: ->

    success: ->

    error: ->